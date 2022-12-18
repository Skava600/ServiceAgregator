import appAxios from "./axios";

const accountPath = "Account";
const profilePath = "Doer";

const getFormData = (data: { [key: string]: any }) => {
    const formData = new FormData();
    Object.entries(data).forEach((element) => {
        formData.append(element[0], element[1]);
    });
    return formData;
};

export const registerUser = async (data: {
    FirstName: string;
    LastName: string;
    Patronym: string;
    Email: string;
    PhoneNumber: string;
    Location: string;
    Password: string;
    PasswordConfirm: string;
}) => {
    const formData = getFormData(data);

    return appAxios({
        url: `${accountPath}/Register`,
        method: "POST",
        data: formData,
        headers: { "Content-Type": "multipart/form-data" },
    });
};

export const loginUser = (data: { Email: string; Password: string }) => {
    const formData = getFormData(data);

    return appAxios({
        url: `${accountPath}/Login`,
        method: "POST",
        data: formData,
        headers: { "Content-Type": "multipart/form-data" },
    });
};

export const getWorkSections = () => {
    return appAxios.get(`/WorkSections/GetListOfSections`);
};

export const getProfiles = (slugs: string[]) => {
    const formData = getFormData(slugs);
    console.log(formData);

    return appAxios({
        url: `${profilePath}/Get`,
        method: "POST",
        data: slugs,
    });
};
