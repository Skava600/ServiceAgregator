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
    Card,
    Grid,
    Divider,
} from "@mui/material";
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import EditIcon from "@mui/icons-material/Edit";
import "./profileInfo.less";

const INITIAL_STATE = {
    passwordConfirmation: { value: "1234", isError: false },
    isPassVisible: false,
    isPass2Visible: false,
    errors: [] as string[],
};

type TProps = {
    user: {
        firstName: string;
        lastName: string;
        patronym: string;
        email: string;
        phoneNubmer: string;
        location: string;
        password: string;
    };
};

export const ProfileInfo = ({ user }: TProps) => {
    const [isEditMode, setIsEditMode] = useState(false);
    const [firstName, setFirstName] = useState({
        value: user.firstName,
        isError: false,
    });
    const [lastName, setLastName] = useState({
        value: user.lastName,
        isError: false,
    });
    const [patronym, setPatronym] = useState({
        value: user.patronym,
        isError: false,
    });
    const [email, setEmail] = useState({ value: user.email, isError: false });
    const [phoneNubmer, setPhoneNubmer] = useState({
        value: user.phoneNubmer,
        isError: false,
    });
    const [location, setLocation] = useState({
        value: user.location,
        isError: false,
    });
    const [password, setPassword] = useState({
        value: user.password,
        isError: false,
    });
    const [passwordConfirmation, setPasswordConfirmation] = useState({
        value: user.password,
        isError: false,
    });
    const [isPassVisible, setIsPassVisible] = useState(
        INITIAL_STATE.isPassVisible
    );
    const [isPass2Visible, setIsPass2Visible] = useState(
        INITIAL_STATE.isPass2Visible
    );
    const [errors, setErrors] = useState(INITIAL_STATE.errors);

    const resetUserInfo = () => {
        setIsEditMode(false);
        setFirstName((v) => ({ ...v, value: user.firstName }));
        setLastName((v) => ({ ...v, value: user.lastName }));
        setPatronym((v) => ({ ...v, value: user.patronym }));
        setEmail((v) => ({ ...v, value: user.email }));
        setPhoneNubmer((v) => ({ ...v, value: user.phoneNubmer }));
        setLocation((v) => ({ ...v, value: user.location }));
        setPassword((v) => ({ ...v, value: user.password }));
        setPasswordConfirmation((v) => ({ ...v, value: user.password }));
    };

    const validateUser = () => {
        let validationMsgs = [];
        if (
            !firstName.value ||
            !lastName.value ||
            !patronym.value ||
            !email.value ||
            !phoneNubmer.value ||
            !password.value ||
            !passwordConfirmation.value
        ) {
            validationMsgs.push("Не заполнены обязательные поля");
        }

        if (password.value !== passwordConfirmation.value) {
            validationMsgs.push("Пароли не совпадают");
            setPassword((value) => ({ ...value, isError: true }));
            setPasswordConfirmation((value) => ({
                ...value,
                isError: true,
            }));
        }

        setFirstName((value) => ({ ...value, isError: !firstName.value }));
        setLastName((value) => ({ ...value, isError: !lastName.value }));
        setPatronym((value) => ({ ...value, isError: !patronym.value }));
        setEmail((value) => ({ ...value, isError: !email.value }));
        setPhoneNubmer((value) => ({ ...value, isError: !phoneNubmer.value }));
        setPassword((value) => ({ ...value, isError: !password.value }));
        setPasswordConfirmation((value) => ({
            ...value,
            isError: !passwordConfirmation.value,
        }));

        return validationMsgs;
    };

    const handleConfirmChanges = () => {
        const errorMsgs = validateUser();

        setErrors(errorMsgs);
        if (errorMsgs.length) return;
    };

    return (
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
                                value={lastName.value}
                                onChange={(e: any) =>
                                    setLastName((prevValue) => ({
                                        ...prevValue,
                                        value: e.target.value,
                                    }))
                                }
                                className="account-info-value-input"
                                variant="outlined"
                                label="Фамилия"
                                error={lastName.isError}
                            />
                        </Grid>
                        <Grid item xs={12} md={4} lg={3}>
                            <TextField
                                size="small"
                                value={firstName.value}
                                onChange={(e: any) =>
                                    setFirstName((prevValue) => ({
                                        ...prevValue,
                                        value: e.target.value,
                                    }))
                                }
                                className="account-info-value-input"
                                variant="outlined"
                                label="Имя"
                                error={firstName.isError}
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
                                value={phoneNubmer.value}
                                onChange={(e: any) =>
                                    setPhoneNubmer((prevValue) => ({
                                        ...prevValue,
                                        value: e.target.value,
                                    }))
                                }
                                className="account-info-value-input"
                                variant="outlined"
                                label="Номер Телефона"
                                error={phoneNubmer.isError}
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
                                label="Пароль"
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
                                label="Подтверждение Пароля"
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
                                    onClick={handleConfirmChanges}
                                >
                                    Сохранить
                                </Button>
                            </Stack>
                        </Grid>
                    </Grid>
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
                                {user.lastName}
                            </div>
                        </Grid>
                        <Grid item xs={12} sm={6} md={4} lg={3}>
                            <b>Имя:</b>
                            <div className="account-info-value">
                                {user.firstName}
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
                                {user.email}
                            </div>
                        </Grid>
                        <Grid item xs={12} sm={6} md={4} lg={3}>
                            <b>Номер телефона:</b>
                            <div className="account-info-value">
                                {user.phoneNubmer}
                            </div>
                        </Grid>
                        <Grid item xs={12} sm={6} md={4} lg={3}>
                            <b>Город:</b>
                            <div className="account-info-value">
                                {user.location}
                            </div>
                        </Grid>
                        <Grid item xs={12} sm={6} md={4} lg={3}>
                            <b>Пароль:</b>
                            <div className="account-info-value">
                                {user.password}
                            </div>
                        </Grid>
                    </Grid>
                </>
            )}
        </Paper>
    );
};
