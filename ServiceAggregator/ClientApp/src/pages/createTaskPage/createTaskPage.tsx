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
import { MobileDatePicker } from "@mui/x-date-pickers/MobileDatePicker";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import PriorityHighIcon from "@mui/icons-material/PriorityHigh";
import { createTask, getWorkSections } from "../../api";
import { IWorkCategory, IWorkSection } from "../../api/interfaces";
import { Errors, Page } from "../../components";
import { getToken } from "../../state/selectors/userSelectors";
import { useAppSelector } from "../../state/store";
import "./createTaskPage.less";

const INITIAL_STATE = {
    header: "",
    text: "",
    location: "",
    expireDate: "",
    price: null,
    slug: null,
    slugsToChoose: [] as IWorkSection[],
    errors: [] as string[],
};

export const CreateTaskPage = () => {
    const [header, setHeader] = useState(INITIAL_STATE.header);
    const [text, setText] = useState(INITIAL_STATE.text);
    const [location, setLocation] = useState(INITIAL_STATE.location);
    const [expireDate, setExpireDate] = useState(INITIAL_STATE.expireDate);
    const [price, setPrice] = useState<number | null>(INITIAL_STATE.price);
    const [slug, setSlug] = useState<IWorkSection | null>(INITIAL_STATE.slug);
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

    const handleLocationChange = (value: string) => {
        setLocation(value);
    };

    const handleExpireDateChange = (date: any) => {
        setExpireDate(date.toString());
    };

    const handlePriceChange = (value: string) => {
        if (+value <= 0 || isNaN(+value)) {
            return;
        }

        setPrice(+value);
    };

    const handleSlugChange = (value: IWorkSection | null) => {
        setSlug(value);
    };

    const validateData = () => {
        let validationMsgs = [];

        if (!header || !text || !location || !expireDate || !slug) {
            validationMsgs.push("Не все поля заполнены");
        }

        return validationMsgs;
    };

    const handleConfirm = () => {
        const errorMsgs = validateData();

        setErrors(errorMsgs);

        console.log({
            header,
            text,
            location,
            expireDate,
            price,
            slug: slug?.slug!,
        });

        if (errorMsgs.length) return;

        createTask(
            {
                header,
                text,
                location,
                expireDate: moment(expireDate).add(4, "hours").utc().format(),
                price: price || "",
                slug: slug?.slug!,
            },
            token!
        ).then(() => navigate("/account"));
    };

    const handleCancel = () => {
        navigate(-1);
    };

    return (
        <LocalizationProvider dateAdapter={AdapterMoment}>
            <Page className="edit-task-page">
                <Paper className="task-paper">
                    <Stack spacing={4}>
                        <Typography variant="h2">Создать заказ</Typography>

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

                        <TextField
                            sx={{ width: "100%" }}
                            value={location}
                            onChange={(e) =>
                                handleLocationChange(e.target.value)
                            }
                            label="Место"
                        />

                        <TextField
                            sx={{ width: "100%" }}
                            value={price}
                            onChange={(e) => handlePriceChange(e.target.value)}
                            label="Цена вопроса"
                            InputProps={{
                                inputProps: { min: 0 },
                                endAdornment: <span>рублей</span>,
                            }}
                            helperText="Оставьте поле пустым, чтобы пометить цену как Договорная"
                        />

                        <MobileDatePicker
                            label="Дата"
                            inputFormat="MM/DD/YYYY"
                            value={expireDate}
                            onChange={handleExpireDateChange}
                            renderInput={(params) => (
                                <TextField {...params} sx={{ width: "100%" }} />
                            )}
                        />

                        <Autocomplete
                            value={slug}
                            onChange={(_, value) => handleSlugChange(value)}
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
