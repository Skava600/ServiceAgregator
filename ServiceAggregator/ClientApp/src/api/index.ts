import appAxios from "./axios";

const accountPath = "Account";
const profilePath = "Doer";
const tasksPath = "Orders";
const responsesPath = "Response";

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
    return appAxios({
        url: `${profilePath}/Get`,
        method: "POST",
        data: slugs,
    });
};

export const getProfile = (data: { id: string }) => {
    return appAxios.get(`${profilePath}/Get`, { params: { id: data.id } });
};

export const getTasks = ({
    slugs,
    isMyOrders,
}: {
    slugs: string[];
    isMyOrders?: string;
}) => {
    return appAxios({
        url: `${tasksPath}/GetPage`,
        method: "POST",
        data: slugs,
        params: { myOrders: isMyOrders },
    });
};

export const getTask = (data: { id: string }) => {
    return appAxios.get(`${tasksPath}/Get/${data.id}`);
};

export const getResponses = (data: { id: string }) => {
    return appAxios.get(`${responsesPath}`, {
        params: { orderId: data.id },
    });
};
