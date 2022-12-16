import { Input } from "@mui/material";
import { useState } from "react";
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

    return (
        <div className="login-page">
            <Input
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
            />
            <Input
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
            />
            <Input
                value={patronym}
                onChange={(e) => setPatronym(e.target.value)}
            />
            <Input value={email} onChange={(e) => setEmail(e.target.value)} />
            <Input
                value={phonenubmer}
                onChange={(e) => setPhoneNubmer(e.target.value)}
            />
            <Input
                value={location}
                onChange={(e) => setLocation(e.target.value)}
            />
            <Input
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <Input
                value={passwordConfirmation}
                onChange={(e) => setPasswordConfirmation(e.target.value)}
            />
        </div>
    );
};
