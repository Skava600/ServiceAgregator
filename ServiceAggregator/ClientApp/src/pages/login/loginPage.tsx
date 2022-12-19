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
import { getAccountInfo, loginUser } from "../../api";
import "./loginPage.less";
import { useAppDispatch } from "../../state/store";
import { loginSuccess } from "../../state/slices/authSlice";
import { userInfo } from "os";
import { Link } from "react-router-dom";

const INITIAL_STATE = {
    email: { value: "blackshark564@gmail.com", isError: false },
    password: { value: "1234", isError: false },
    isPassVisible: false,
    errors: [] as string[],
};

export const LoginPage = () => {
    const [email, setEmail] = useState(INITIAL_STATE.email);
    const [password, setPassword] = useState(INITIAL_STATE.password);
    const [isPassVisible, setIsPassVisible] = useState(
        INITIAL_STATE.isPassVisible
    );
    const [errors, setErrors] = useState(INITIAL_STATE.errors);

    const dispatch = useAppDispatch();

    const navigate = useNavigate();

    const validateUser = () => {
        let validationMsgs = [];

        if (!email.value) {
            validationMsgs.push("Укажите логин");
            setEmail((value) => ({ ...value, isError: true }));
        }
        if (!password.value) {
            validationMsgs.push("Укажите пароль");
            setPassword((value) => ({ ...value, isError: true }));
        }

        return validationMsgs;
    };

    const handleLogin = () => {
        const errorMsgs = validateUser();

        setErrors(errorMsgs);
        if (errorMsgs.length) return;

        loginUser({
            Email: email.value,
            Password: password.value,
        }).then(({ data }) => {
            const { success, errors } = data;
            if (!success && success !== undefined) {
                setErrors(errors);
            } else {
                navigate("/");
                const token = data;
                getAccountInfo(token).then(({ data }) => {
                    const {
                        firstname,
                        lastname,
                        location,
                        login,
                        patronym,
                        phonenumber,
                    } = data;
                    dispatch(
                        loginSuccess({
                            user: {
                                firstname,
                                lastname,
                                location,
                                login,
                                patronym,
                                phonenumber,
                            },
                            token,
                        })
                    );
                });
            }
        });
    };

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
                    <h1 className="form-title">Вход</h1>
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
                        label="Email"
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
                        label="Пароль"
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
                    <Stack spacing={1} direction="column" className="actions">
                        {errors.map((err) => (
                            <div className="error">
                                <PriorityHighIcon />
                                <Typography color="red" sx={{ width: "100%" }}>
                                    {err}
                                </Typography>
                            </div>
                        ))}
                    </Stack>

                    <Stack spacing={15} direction="row" className="actions">
                        <Button variant="outlined" onClick={handleCancel}>
                            Отмена
                        </Button>
                        <Button variant="contained" onClick={handleLogin}>
                            Войти
                        </Button>
                    </Stack>
                    <Stack spacing={5} direction="row" className="actions">
                        <Typography color="white" sx={{ width: "100%" }}>
                            Нет аккаунта?
                        </Typography>
                        <Typography color="white" sx={{ width: "100%" }}>
                            <Link to="/register">Зарегистрируйтесь</Link>
                        </Typography>
                    </Stack>
                </Paper>
            </div>
        </div>
    );
};
