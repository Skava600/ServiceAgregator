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
    getProfile,
    getWorkSections,
    updateProfile,
} from "../../api";
import { IWorkCategory, IWorkSection } from "../../api/interfaces";
import { getToken } from "../../state/selectors/userSelectors";
import { useAppSelector } from "../../state/store";
import { Errors, SectionBadges, Page } from "../../components/";
import "./createProfilePage.less";

const INITIAL_STATE = {
    doerName: "",
    doerDescription: "",
    location: "",
    expireDate: "",
    price: null,
    sections: [],
    slugsToChoose: [] as IWorkSection[],
    errors: [] as string[],
};

export const CreateProfilePage = () => {
    const { id } = useParams();
    const [doerName, setDoerName] = useState(INITIAL_STATE.doerName);
    const [doerDescription, setDoerDescription] = useState(
        INITIAL_STATE.doerDescription
    );
    const [sections, setSections] = useState<IWorkSection[]>(
        INITIAL_STATE.sections
    );
    const [slugsToChoose, setSlugsToChoose] = useState(
        INITIAL_STATE.slugsToChoose
    );
    const [errors, setErrors] = useState(INITIAL_STATE.errors);
    const navigate = useNavigate();
    const token = useAppSelector(getToken);

    const isEditMode = !!id;

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

        if (isEditMode) {
            getProfile({ id }).then(({ data }) => {
                setDoerName(data.doerName);
                setDoerDescription(data.doerDescription);
                setSections(data.sections);
            });
        }
    }, [id, isEditMode]);

    const handleDoerNameChange = (value: string) => {
        setDoerName(value);
    };

    const handleDoerDescriptionChange = (value: string) => {
        setDoerDescription(value);
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

        if (!doerName || !doerDescription || !sections.length) {
            validationMsgs.push("Не все поля заполнены");
        }

        return validationMsgs;
    };

    const handleConfirm = () => {
        const errorMsgs = validateData();

        setErrors(errorMsgs);

        if (errorMsgs.length) return;

        if (isEditMode) {
            updateProfile(
                { doerName, doerDescription, id },
                sections.map(({ slug }) => slug),
                token!
            ).then(({ data }) => {
                if (!data.success) {
                    setErrors(data.errors);
                    return;
                }

                navigate("/profiles");
            });
        } else {
            createProfile(
                { doerName, doerDescription },
                sections.map(({ slug }) => slug),
                token!
            ).then(({ data }) => {
                if (!data.success) {
                    setErrors(data.errors);
                    return;
                }

                navigate("/profiles");
            });
        }
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
                            {isEditMode
                                ? "Мой профиль исполнителя"
                                : "Создать профиль исполнителя"}
                        </Typography>

                        <TextField
                            sx={{ width: "100%" }}
                            multiline
                            value={doerName}
                            onChange={(e) =>
                                handleDoerNameChange(e.target.value)
                            }
                            label="Заглавие"
                        />

                        <TextField
                            sx={{ width: "100%" }}
                            multiline
                            value={doerDescription}
                            onChange={(e) =>
                                handleDoerDescriptionChange(e.target.value)
                            }
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

                        <SectionBadges
                            sectionsList={sections}
                            onRemoveSection={handleSlugChange}
                        />

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
                                {isEditMode ? "Сохранить изменения" : "Создать"}
                            </Button>
                        </Stack>
                    </Stack>
                </Paper>
            </Page>
        </LocalizationProvider>
    );
};
