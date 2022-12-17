import { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    TextField,
    Paper,
    Button,
    Stack,
    IconButton,
    InputAdornment,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import "./loginPage.less";

const INITIAL_STATE = {
    email: "",
    password: "",
    isPassVisible: false,
};

export const LoginPage = () => {
    const [email, setEmail] = useState(INITIAL_STATE.email);
    const [password, setPassword] = useState(INITIAL_STATE.password);
    const [isPassVisible, setIsPassVisible] = useState(
        INITIAL_STATE.isPassVisible
    );

    const navigate = useNavigate();

    const handleRegister = () => {};

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
                        value={email}
                        onChange={(e: any) => setEmail(e.target.value)}
                        className="form-input"
                        variant="outlined"
                        label="Email"
                    />
                    <TextField
                        size="small"
                        value={password}
                        onChange={(e: any) => setPassword(e.target.value)}
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

                    <Stack spacing={15} direction="row" className="actions">
                        <Button variant="outlined" onClick={handleCancel}>
                            Отмена
                        </Button>
                        <Button variant="contained" onClick={handleRegister}>
                            Войти
                        </Button>
                    </Stack>
                </Paper>
            </div>
        </div>
    );
};
