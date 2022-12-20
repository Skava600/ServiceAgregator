import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import moment from "moment";
import {
    Autocomplete,
    Box,
    Button,
    Grid,
    Paper,
    Stack,
    TextField,
    Typography,
} from "@mui/material";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider/LocalizationProvider";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import {
    createProfile,
    createTask,
    getTask,
    getWorkSections,
    updateTask,
} from "../../api";
import { IWorkCategory, IWorkSection } from "../../api/interfaces";
import { Page } from "../../components";
import { getToken } from "../../state/selectors/userSelectors";
import { useAppSelector } from "../../state/store";
import { Errors } from "../../components/";
import "./createProfilePage.less";

const INITIAL_STATE = {
    header: "",
    text: "",
    location: "",
    expireDate: "",
    price: null,
    sections: [],
    slugsToChoose: [] as IWorkSection[],
    errors: [] as string[],
};

export const CreateProfilePage = () => {
    const [header, setHeader] = useState(INITIAL_STATE.header);
    const [text, setText] = useState(INITIAL_STATE.text);
    const [sections, setSections] = useState<IWorkSection[]>(
        INITIAL_STATE.sections
    );
    const [slugsToChoose, setSlugsToChoose] = useState(
        INITIAL_STATE.slugsToChoose
    );
    const [errors, setErrors] = useState(INITIAL_STATE.errors);
    const navigate = useNavigate();
    const token = useAppSelector(getToken);

    useEffect(() => {
        getWorkSections().then(({ data }) =>
            setSlugsToChoose(
                data.reduce(
                    (t: IWorkSection[], c: IWorkCategory) => [
                        ...t,
                        ...c.sections,
                    ],
                    [] as IWorkSection[]
                )
            )
        );
    }, []);

    const handleHeaderChange = (value: string) => {
        setHeader(value);
    };

    const handleTextChange = (value: string) => {
        setText(value);
    };

    const handleSlugChange = (section: IWorkSection) => {
        const index = sections?.findIndex((s) => s.slug === section.slug);
        const exists = index !== -1;

        if (!exists) {
            setSections((v) => [...v, section]);
            return;
        } else {
            const _sections = [...sections];
            _sections.splice(index, 1);
            setSections(_sections);
        }
    };

    const validateData = () => {
        let validationMsgs = [];

        if (!header || !text || !sections.length) {
            validationMsgs.push("Не все поля заполнены");
        }

        return validationMsgs;
    };

    const handleConfirm = () => {
        const errorMsgs = validateData();

        setErrors(errorMsgs);

        if (errorMsgs.length) return;

        createProfile(
            { header, text },
            sections.map(({ slug }) => slug),
            token!
        ).then(({ data }) => {
            if (!data.success) {
                setErrors(data.errors);
            }
        });
    };

    const handleCancel = () => {
        navigate(-1);
    };

    return (
        <LocalizationProvider dateAdapter={AdapterMoment}>
            <Page className="edit-task-page">
                <Paper className="task-paper">
                    <Stack spacing={4}>
                        <Typography variant="h2">
                            Создать профиль исполнителя
                        </Typography>

                        <TextField
                            sx={{ width: "100%" }}
                            multiline
                            value={header}
                            onChange={(e) => handleHeaderChange(e.target.value)}
                            label="Заглавие"
                        />

                        <TextField
                            sx={{ width: "100%" }}
                            multiline
                            value={text}
                            onChange={(e) => handleTextChange(e.target.value)}
                            label="Описание"
                        />

                        <Autocomplete
                            onChange={(_, value) => handleSlugChange(value!)}
                            disablePortal
                            options={slugsToChoose}
                            sx={{ width: "100%" }}
                            getOptionLabel={(option) => option.name}
                            renderInput={(params) => {
                                return (
                                    <TextField
                                        sx={{ width: "100%" }}
                                        {...params}
                                    />
                                );
                            }}
                        />

                        <Stack>{sections.map(({ name }) => name)}</Stack>

                        <Errors errors={errors} />

                        <Stack
                            direction="row"
                            width="100%"
                            justifyContent="space-between"
                        >
                            <Button variant="outlined" onClick={handleCancel}>
                                Отмена
                            </Button>
                            <Button variant="contained" onClick={handleConfirm}>
                                Создать
                            </Button>
                        </Stack>
                    </Stack>
                </Paper>
            </Page>
        </LocalizationProvider>
    );
};
