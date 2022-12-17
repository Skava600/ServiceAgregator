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
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import PriorityHighIcon from "@mui/icons-material/PriorityHigh";
import { registerUser } from "./login.api";
import "./loginPage.less";

const INITIAL_STATE = {
    firstName: { value: "", isError: false },
    lastName: { value: "", isError: false },
    patronym: { value: "", isError: false },
    email: { value: "", isError: false },
    phoneNubmer: { value: "", isError: false },
    location: { value: "", isError: false },
    password: { value: "", isError: false },
    passwordConfirmation: { value: "", isError: false },
    isPassVisible: false,
    isPass2Visible: false,
    errors: [] as string[],
};

export const RegisterPage = () => {
    const [firstName, setFirstName] = useState(INITIAL_STATE.firstName);
    const [lastName, setLastName] = useState(INITIAL_STATE.lastName);
    const [patronym, setPatronym] = useState(INITIAL_STATE.patronym);
    const [email, setEmail] = useState(INITIAL_STATE.email);
    const [phonenubmer, setPhoneNubmer] = useState(INITIAL_STATE.phoneNubmer);
    const [location, setLocation] = useState(INITIAL_STATE.location);
    const [password, setPassword] = useState(INITIAL_STATE.password);
    const [passwordConfirmation, setPasswordConfirmation] = useState(
        INITIAL_STATE.passwordConfirmation
    );
    const [isPassVisible, setIsPassVisible] = useState(
        INITIAL_STATE.isPassVisible
    );
    const [isPass2Visible, setIsPass2Visible] = useState(
        INITIAL_STATE.isPass2Visible
    );
    const [errors, setErrors] = useState(INITIAL_STATE.errors);

    const validateUser = () => {
        let validationMsgs = [];
        if (
            !firstName.value ||
            !lastName.value ||
            !patronym.value ||
            !email.value ||
            !phonenubmer.value ||
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
        setPhoneNubmer((value) => ({ ...value, isError: !phonenubmer.value }));
        setPassword((value) => ({ ...value, isError: !password.value }));
        setPasswordConfirmation((value) => ({
            ...value,
            isError: !passwordConfirmation.value,
        }));

        return validationMsgs;
    };

    const handleRegister = () => {
        const errorMsgs = validateUser();
        console.log(errors);
        console.log(email);

        setErrors(errorMsgs);
        if (errorMsgs.length) return;

        registerUser({
            FirstName: firstName.value,
            LastName: lastName.value,
            Patronym: patronym.value,
            Email: email.value,
            PhoneNumber: phonenubmer.value,
            Location: location.value,
            Password: password.value,
            PasswordConfirm: password.value,
        });
    };
    const navigate = useNavigate();

    const handleCancel = () => {
        navigate(-1);
    };

    return (
        <div className="login-page">
            <div>
                <div className="login-header">
                    <h1 className="form-title">Service Agregator</h1>
                    <IconButton
                        aria-label="cancel"
                        size="large"
                        onClick={handleCancel}
                    >
                        <CloseIcon fontSize="inherit" />
                    </IconButton>
                </div>
                <Paper className="content" elevation={5}>
                    <h1 className="form-title">Регистрация</h1>
                    <TextField
                        size="small"
                        value={firstName.value}
                        onChange={(e: any) =>
                            setFirstName((prevValue) => ({
                                ...prevValue,
                                value: e.target.value,
                            }))
                        }
                        className="form-input"
                        variant="outlined"
                        label="Имя *"
                        error={firstName.isError}
                    />
                    <TextField
                        size="small"
                        value={lastName.value}
                        onChange={(e: any) =>
                            setLastName((prevValue) => ({
                                ...prevValue,
                                value: e.target.value,
                            }))
                        }
                        className="form-input"
                        variant="outlined"
                        label="Фамилия *"
                        error={lastName.isError}
                    />
                    <TextField
                        size="small"
                        value={patronym.value}
                        onChange={(e: any) =>
                            setPatronym((prevValue) => ({
                                ...prevValue,
                                value: e.target.value,
                            }))
                        }
                        className="form-input"
                        variant="outlined"
                        label="Отчество *"
                        error={patronym.isError}
                    />
                    <TextField
                        size="small"
                        value={email.value}
                        onChange={(e: any) =>
                            setEmail((prevValue) => ({
                                ...prevValue,
                                value: e.target.value,
                            }))
                        }
                        className="form-input"
                        variant="outlined"
                        label="Email *"
                        error={email.isError}
                    />
                    <TextField
                        size="small"
                        value={password.value}
                        onChange={(e: any) =>
                            setPassword((prevValue) => ({
                                ...prevValue,
                                value: e.target.value,
                            }))
                        }
                        className="form-input"
                        variant="outlined"
                        label="Пароль *"
                        error={password.isError}
                        type={isPassVisible ? "" : "password"}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <IconButton
                                        onClick={() =>
                                            setIsPassVisible((value) => !value)
                                        }
                                        onMouseDown={() =>
                                            setIsPassVisible((value) => !value)
                                        }
                                        aria-label="showPass"
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
                    <TextField
                        size="small"
                        value={passwordConfirmation.value}
                        onChange={(e: any) =>
                            setPasswordConfirmation((prevValue) => ({
                                ...prevValue,
                                value: e.target.value,
                            }))
                        }
                        className="form-input"
                        variant="outlined"
                        label="Подтверждение Пароля *"
                        error={passwordConfirmation.isError}
                        type={isPass2Visible ? "" : "password"}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <IconButton
                                        onClick={() =>
                                            setIsPass2Visible((value) => !value)
                                        }
                                        onMouseDown={() =>
                                            setIsPass2Visible((value) => !value)
                                        }
                                        aria-label="showPass"
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
                    <TextField
                        size="small"
                        value={phonenubmer.value}
                        onChange={(e: any) =>
                            setPhoneNubmer((prevValue) => ({
                                ...prevValue,
                                value: e.target.value,
                            }))
                        }
                        className="form-input"
                        variant="outlined"
                        label="Номер Телефона *"
                        error={phonenubmer.isError}
                    />
                    <TextField
                        size="small"
                        value={location.value}
                        onChange={(e: any) =>
                            setLocation((prevValue) => ({
                                ...prevValue,
                                value: e.target.value,
                            }))
                        }
                        className="form-input"
                        variant="outlined"
                        label="Город"
                        error={location.isError}
                    />
                    <Stack spacing={2} direction="column" className="actions">
                        {errors.map((err) => (
                            <div className="error">
                                <PriorityHighIcon />
                                <Typography color="red">{err}</Typography>
                            </div>
                        ))}
                    </Stack>
                    <Stack spacing={2} direction="row" className="actions">
                        <Button variant="outlined" onClick={handleCancel}>
                            Отмена
                        </Button>
                        <Button variant="contained" onClick={handleRegister}>
                            Зарегистрироваться
                        </Button>
                    </Stack>
                </Paper>
            </div>
        </div>
    );
};
