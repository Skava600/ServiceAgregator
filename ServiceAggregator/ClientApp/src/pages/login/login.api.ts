import appAxios from "../../axios";

const path = "Account";

export const registerUser = (body: {
    FirstName: string;
    LastName: string;
    Patronym: string;
    Email: string;
    PhoneNumber: string;
    Location: string;
    Password: string;
    PasswordConfirm: string;
}) => {
    appAxios.post(`${path}/Register`, { body });
};
