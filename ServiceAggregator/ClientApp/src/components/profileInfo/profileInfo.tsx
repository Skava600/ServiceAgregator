import { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    TextField,
    Paper,
    Button,
    Stack,
    IconButton,
    InputAdornment,
    Typography,
    Grid,
    Divider,
} from "@mui/material";
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import EditIcon from "@mui/icons-material/Edit";
import PriorityHighIcon from "@mui/icons-material/PriorityHigh";
import { IUser } from "../../api/interfaces";
import { getToken } from "../../state/selectors/userSelectors";
import { useAppDispatch, useAppSelector } from "../../state/store";
import {
    getAccountInfo,
    updateAccountInfo,
    updateAccountPassword,
} from "../../api";
import "./profileInfo.less";
import { setUser } from "../../state/slices/authSlice";

const INITIAL_STATE = {
    passwordConfirmation: { value: "1234", isError: false },
    isPassVisible: false,
    isPass2Visible: false,
    errors: [] as string[],
};

type TProps = {
    user: IUser;
};

export const ProfileInfo = ({ user }: TProps) => {
    const [isEditMode, setIsEditMode] = useState(false);
    const [firstname, setFirstName] = useState({
        value: user.firstname,
        isError: false,
    });
    const [lastname, setLastName] = useState({
        value: user.lastname,
        isError: false,
    });
    const [patronym, setPatronym] = useState({
        value: user.patronym,
        isError: false,
    });
    const [email, setEmail] = useState({ value: user.login, isError: false });
    const [phonenumber, setPhoneNubmer] = useState({
        value: user.phoneNumber,
        isError: false,
    });
    const [location, setLocation] = useState({
        value: user.location,
        isError: false,
    });
    const [password, setPassword] = useState({
        value: "",
        isError: false,
    });
    const [passwordConfirmation, setPasswordConfirmation] = useState({
        value: "",
        isError: false,
    });
    const [isPassVisible, setIsPassVisible] = useState(
        INITIAL_STATE.isPassVisible
    );
    const [isPass2Visible, setIsPass2Visible] = useState(
        INITIAL_STATE.isPass2Visible
    );
    const [errors, setErrors] = useState(INITIAL_STATE.errors);
    const [passwordErrors, setPasswordErrors] = useState(INITIAL_STATE.errors);
    const dispatch = useAppDispatch();

    const resetUserInfo = () => {
        setIsEditMode(false);
        setFirstName((v) => ({ ...v, value: user.firstname }));
        setLastName((v) => ({ ...v, value: user.lastname }));
        setPatronym((v) => ({ ...v, value: user.patronym }));
        setEmail((v) => ({ ...v, value: user.login }));
        setPhoneNubmer((v) => ({ ...v, value: user.phoneNumber }));
        setLocation((v) => ({ ...v, value: user.location }));
        setPassword((v) => ({ ...v, value: "" }));
        setPasswordConfirmation((v) => ({ ...v, value: "" }));
    };
    const token = useAppSelector(getToken);

    const validateUser = () => {
        let validationMsgs = [];
        if (
            !firstname.value ||
            !lastname.value ||
            !patronym.value ||
            !email.value ||
            !phonenumber.value
        ) {
            validationMsgs.push("Не заполнены обязательные поля");
        }

        // if (password.value !== passwordConfirmation.value) {
        //     validationMsgs.push("Пароли не совпадают");
        //     setPassword((value) => ({ ...value, isError: true }));
        //     setPasswordConfirmation((value) => ({
        //         ...value,
        //         isError: true,
        //     }));
        // }

        setFirstName((value) => ({ ...value, isError: !firstname.value }));
        setLastName((value) => ({ ...value, isError: !lastname.value }));
        setPatronym((value) => ({ ...value, isError: !patronym.value }));
        setEmail((value) => ({ ...value, isError: !email.value }));
        setPhoneNubmer((value) => ({ ...value, isError: !phonenumber.value }));

        return validationMsgs;
    };

    const handleConfirmChanges = () => {
        const errorMsgs = validateUser();

        setErrors(errorMsgs);

        if (errorMsgs.length) return;

        updateAccountInfo(
            {
                firstname: firstname.value,
                lastname: lastname.value,
                patronym: patronym.value,
                phoneNumber: phonenumber.value,
                location: location.value,
            },
            token!
        ).then(({ data }) => {
            if (!data.success) {
                setPasswordErrors(data.errors);
                return;
            }

            getAccountInfo(token!).then(({ data }) => {
                return dispatch(setUser({ user: data as IUser }));
                setIsEditMode(false);
            });
        });
    };

    const validatePasswords = () => {
        let validationMsgs = [];
        if (!password.value || !passwordConfirmation.value) {
            validationMsgs.push("Заполните оба поля паролей");

            setPassword((value) => ({ ...value, isError: !password.value }));
            setPasswordConfirmation((value) => ({
                ...value,
                isError: !passwordConfirmation.value,
            }));
        }

        return validationMsgs;
    };

    const handleChangePassword = () => {
        const errorMsgs = validatePasswords();

        setPasswordErrors(errorMsgs);

        if (errorMsgs.length) return;

        updateAccountPassword(
            {
                oldPassword: password.value,
                newPassword: passwordConfirmation.value,
            },
            token!
        ).then(({ data }) => {
            if (!data.success) {
                setPasswordErrors(data.errors);
                return;
            }

            setIsEditMode(false);
        });
    };

    return (
        <>
            <Paper className="profile-info-card">
                {isEditMode ? (
                    <>
                        <div className="account-info-header">
                            <h1 className="account-info-title">
                                Изменить информацию об аккаунте
                            </h1>
                            <IconButton onClick={resetUserInfo}>
                                <EditIcon />
                            </IconButton>
                        </div>
                        <Divider />
                        <Grid container spacing={2} className="account-info">
                            <Grid item xs={12} md={4} lg={3}>
                                <TextField
                                    size="small"
                                    value={lastname.value}
                                    onChange={(e: any) =>
                                        setLastName((prevValue) => ({
                                            ...prevValue,
                                            value: e.target.value,
                                        }))
                                    }
                                    className="account-info-value-input"
                                    variant="outlined"
                                    label="Фамилия"
                                    error={lastname.isError}
                                />
                            </Grid>
                            <Grid item xs={12} md={4} lg={3}>
                                <TextField
                                    size="small"
                                    value={firstname.value}
                                    onChange={(e: any) =>
                                        setFirstName((prevValue) => ({
                                            ...prevValue,
                                            value: e.target.value,
                                        }))
                                    }
                                    className="account-info-value-input"
                                    variant="outlined"
                                    label="Имя"
                                    error={firstname.isError}
                                />
                            </Grid>
                            <Grid item xs={12} md={4} lg={3}>
                                <TextField
                                    size="small"
                                    value={patronym.value}
                                    onChange={(e: any) =>
                                        setPatronym((prevValue) => ({
                                            ...prevValue,
                                            value: e.target.value,
                                        }))
                                    }
                                    className="account-info-value-input"
                                    variant="outlined"
                                    label="Отчество"
                                    error={patronym.isError}
                                />
                            </Grid>
                            <Grid item xs={12} md={4} lg={3}>
                                <TextField
                                    size="small"
                                    value={phonenumber.value}
                                    onChange={(e: any) =>
                                        setPhoneNubmer((prevValue) => ({
                                            ...prevValue,
                                            value: e.target.value,
                                        }))
                                    }
                                    className="account-info-value-input"
                                    variant="outlined"
                                    label="Номер Телефона"
                                    error={phonenumber.isError}
                                />
                            </Grid>
                            <Grid item xs={12} md={4} lg={3}>
                                <TextField
                                    size="small"
                                    value={location.value}
                                    onChange={(e: any) =>
                                        setLocation((prevValue) => ({
                                            ...prevValue,
                                            value: e.target.value,
                                        }))
                                    }
                                    className="account-info-value-input"
                                    variant="outlined"
                                    label="Город"
                                    error={location.isError}
                                />
                            </Grid>

                            <Grid item xs={12} md={4} lg={3}>
                                <Stack direction="row" spacing={5}>
                                    <Button
                                        variant="outlined"
                                        onClick={resetUserInfo}
                                    >
                                        Отмена
                                    </Button>
                                    <Button
                                        variant="contained"
                                        onClick={handleConfirmChanges}
                                    >
                                        Сохранить
                                    </Button>
                                </Stack>
                            </Grid>
                        </Grid>
                        <Stack
                            spacing={1}
                            direction="column"
                            className="errors"
                            sx={{ marginTop: 10 }}
                        >
                            {errors.map((err) => (
                                <div className="error">
                                    <PriorityHighIcon />
                                    <Typography
                                        color="red"
                                        sx={{ width: "100%" }}
                                    >
                                        {err}
                                    </Typography>
                                </div>
                            ))}
                        </Stack>
                    </>
                ) : (
                    <>
                        <div className="account-info-header">
                            <h1 className="account-info-title">
                                Информация об аккаунте
                            </h1>
                            <IconButton onClick={() => setIsEditMode(true)}>
                                <EditIcon />
                            </IconButton>
                        </div>
                        <Divider />
                        <Grid container spacing={2} className="account-info">
                            <Grid item xs={12} sm={6} md={4} lg={3}>
                                <b>Фамилия:</b>
                                <div className="account-info-value">
                                    {user.lastname}
                                </div>
                            </Grid>
                            <Grid item xs={12} sm={6} md={4} lg={3}>
                                <b>Имя:</b>
                                <div className="account-info-value">
                                    {user.firstname}
                                </div>
                            </Grid>
                            <Grid item xs={12} sm={6} md={4} lg={3}>
                                <b>Отчество:</b>
                                <div className="account-info-value">
                                    {user.patronym}
                                </div>
                            </Grid>
                            <Grid item xs={12} sm={6} md={4} lg={3}>
                                <b>Email:</b>
                                <div className="account-info-value">
                                    {user.login}
                                </div>
                            </Grid>
                            <Grid item xs={12} sm={6} md={4} lg={3}>
                                <b>Номер телефона:</b>
                                <div className="account-info-value">
                                    {user.phoneNumber}
                                </div>
                            </Grid>
                            <Grid item xs={12} sm={6} md={4} lg={3}>
                                <b>Город:</b>
                                <div className="account-info-value">
                                    {user.location}
                                </div>
                            </Grid>
                        </Grid>
                    </>
                )}
            </Paper>
            {isEditMode && (
                <Paper className="profile-info-card">
                    <div className="account-info-header">
                        <h1 className="account-info-title">Изменить пароль</h1>
                    </div>
                    <Divider />
                    <Grid container spacing={2} className="account-info">
                        <Grid item xs={12} md={4} lg={3}>
                            <TextField
                                size="small"
                                value={password.value}
                                onChange={(e: any) =>
                                    setPassword((prevValue) => ({
                                        ...prevValue,
                                        value: e.target.value,
                                    }))
                                }
                                className="account-info-value-input"
                                variant="outlined"
                                label="Старый Пароль"
                                error={password.isError}
                                type={isPassVisible ? "" : "password"}
                                InputProps={{
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <IconButton
                                                onClick={() =>
                                                    setIsPassVisible(
                                                        (value) => !value
                                                    )
                                                }
                                                onMouseDown={() =>
                                                    setIsPassVisible(
                                                        (value) => !value
                                                    )
                                                }
                                                aria-label="show password"
                                                size="small"
                                            >
                                                {isPassVisible ? (
                                                    <VisibilityIcon />
                                                ) : (
                                                    <VisibilityOffIcon />
                                                )}
                                            </IconButton>
                                        </InputAdornment>
                                    ),
                                }}
                            />
                        </Grid>
                        <Grid item xs={12} md={4} lg={3}>
                            <TextField
                                size="small"
                                value={passwordConfirmation.value}
                                onChange={(e: any) =>
                                    setPasswordConfirmation((prevValue) => ({
                                        ...prevValue,
                                        value: e.target.value,
                                    }))
                                }
                                className="account-info-value-input"
                                variant="outlined"
                                label="Новый Пароль"
                                error={passwordConfirmation.isError}
                                type={isPass2Visible ? "" : "password"}
                                InputProps={{
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <IconButton
                                                onClick={() =>
                                                    setIsPass2Visible(
                                                        (value) => !value
                                                    )
                                                }
                                                onMouseDown={() =>
                                                    setIsPass2Visible(
                                                        (value) => !value
                                                    )
                                                }
                                                aria-label="show password"
                                                size="small"
                                            >
                                                {isPass2Visible ? (
                                                    <VisibilityIcon />
                                                ) : (
                                                    <VisibilityOffIcon />
                                                )}
                                            </IconButton>
                                        </InputAdornment>
                                    ),
                                }}
                            />
                        </Grid>
                        <Grid item xs={12} md={4} lg={3}>
                            <Stack direction="row" spacing={5}>
                                <Button
                                    variant="outlined"
                                    onClick={resetUserInfo}
                                >
                                    Отмена
                                </Button>
                                <Button
                                    variant="contained"
                                    onClick={handleChangePassword}
                                >
                                    Сохранить
                                </Button>
                            </Stack>
                        </Grid>
                    </Grid>
                    <Stack
                        spacing={1}
                        direction="column"
                        className="errors"
                        sx={{ marginTop: 10 }}
                    >
                        {passwordErrors.map((err) => (
                            <div className="error">
                                <PriorityHighIcon />
                                <Typography color="red" sx={{ width: "100%" }}>
                                    {err}
                                </Typography>
                            </div>
                        ))}
                    </Stack>
                </Paper>
            )}
        </>
    );
};
