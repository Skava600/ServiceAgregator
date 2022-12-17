import { TextField, Paper, Button, Stack, IconButton } from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { useState } from "react";
import { registerUser } from "./login.api";
import "./loginPage.less";

const INITIAL_STATE = {
    firstName: "",
    lastName: "",
    patronym: "",
    email: "",
    phoneNubmer: "",
    location: "",
    password: "",
    passwordConfirmation: "",
    isPassVisible: false,
    isPass2Visible: false,
};

export const LoginPage = () => {
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

    const handleRegister = () => {
        registerUser({
            FirstName: firstName,
            LastName: lastName,
            Patronym: patronym,
            Email: email,
            PhoneNumber: phonenubmer,
            Location: location,
            Password: password,
            PasswordConfirm: password,
        });
    };

    return (
        <div className="login-page">
            <div>
                <div>
                    <h1 className="form-title">Service Agregator</h1>
                    <IconButton aria-label="delete" size="small">
                        <CloseIcon fontSize="inherit" />
                    </IconButton>
                </div>
                <Paper className="content" elevation={5}>
                    <h1 className="form-title">Регистрация</h1>
                    <TextField
                        size="small"
                        value={firstName}
                        onChange={(e: any) => setFirstName(e.target.value)}
                        className="form-input"
                        variant="outlined"
                        label="Имя *"
                    />
                    <TextField
                        size="small"
                        value={lastName}
                        onChange={(e: any) => setLastName(e.target.value)}
                        className="form-input"
                        variant="outlined"
                        label="Фамилия *"
                    />
                    <TextField
                        size="small"
                        value={patronym}
                        onChange={(e: any) => setPatronym(e.target.value)}
                        className="form-input"
                        variant="outlined"
                        label="Отчество *"
                    />
                    <TextField
                        size="small"
                        value={email}
                        onChange={(e: any) => setEmail(e.target.value)}
                        className="form-input"
                        variant="outlined"
                        label="Email *"
                    />
                    <TextField
                        size="small"
                        value={phonenubmer}
                        onChange={(e: any) => setPhoneNubmer(e.target.value)}
                        className="form-input"
                        variant="outlined"
                        label="Номер Телефона *"
                    />
                    <TextField
                        size="small"
                        value={password}
                        onChange={(e: any) => setPassword(e.target.value)}
                        className="form-input"
                        variant="outlined"
                        label="Пароль *"
                        type=""
                    />
                    <TextField
                        size="small"
                        value={passwordConfirmation}
                        onChange={(e: any) =>
                            setPasswordConfirmation(e.target.value)
                        }
                        className="form-input"
                        variant="outlined"
                        label="Подтверждение Пароля *"
                        type="password"
                    />
                    <TextField
                        size="small"
                        value={location}
                        onChange={(e: any) => setLocation(e.target.value)}
                        className="form-input"
                        variant="outlined"
                        label="Город"
                    />

                    <Stack spacing={2} direction="row" className="actions">
                        <Button variant="outlined">Отмена</Button>
                        <Button variant="contained" onClick={handleRegister}>
                            Зарегистрироваться
                        </Button>
                    </Stack>
                </Paper>
            </div>
        </div>
    );
};
