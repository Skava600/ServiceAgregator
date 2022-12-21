import appAxios from "./axios";
import { IProfile, ITask, IUser } from "./interfaces";

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
        headers: {
            "Content-Type": "multipart/form-data",
        },
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
    token,
}: {
    slugs: string[];
    isMyOrders?: boolean;
    token?: string | null;
}) => {
    return appAxios({
        url: `${tasksPath}/GetPage`,
        method: "POST",
        data: slugs,
        params: { myOrders: isMyOrders },
        headers: {
            Authorization: `Bearer ${token || ""}`,
        },
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

export const logoutAccount = (token: string) => {
    return appAxios.get(`${accountPath}/Logout`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const getAccountInfo = (token: string) => {
    return appAxios.get(`${accountPath}/Info`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const updateAccountInfo = (user: Partial<IUser>, token: string) => {
    const formData = getFormData(user);

    return appAxios({
        url: `${accountPath}/UpdateAccountInfo`,
        method: "POST",
        data: formData,
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const updateAccountPassword = (
    passwords: { oldPassword: string; newPassword: string },
    token: string
) => {
    const formData = getFormData(passwords);

    return appAxios({
        url: `${accountPath}/ChangeAccountPassword`,
        method: "POST",
        data: formData,
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const createTask = (
    data: {
        header: string;
        text: string;
        location: string;
        expireDate: string;
        price: number | null;
        slug: string;
    },
    token: string
) => {
    const formData = getFormData(data);

    return appAxios({
        url: `${tasksPath}/MakeOrder`,
        method: "POST",
        data: formData,
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};
export const updateTask = (
    id: string,
    data: {
        header: string;
        text: string;
        location: string;
        expireDate: string;
        price: number | null;
        slug: string;
    },
    token: string
) => {
    const formData = getFormData(data);

    return appAxios({
        url: `${tasksPath}/UpdateOrder`,
        method: "PUT",
        data: formData,
        params: { orderId: id },
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const createProfile = (
    data: Pick<IProfile, "doerName" | "doerDescription">,
    slugs: string[],
    token: string
) => {
    const formData = getFormData({
        doerName: data.doerName,
        doerDescription: data.doerDescription,
        filters: slugs,
    });

    return appAxios({
        url: `${profilePath}/CreateDoerAccount`,
        method: "POST",
        data: formData,
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const updateProfile = (
    data: Pick<IProfile, "doerName" | "doerDescription" | "id">,
    slugs: string[],
    token: string
) => {
    const formData = getFormData({
        doerName: data.doerName,
        doerDescription: data.doerDescription,
        filters: slugs,
    });

    return appAxios({
        url: `${profilePath}/Put/${data.id}`,
        method: "PUT",
        data: formData,
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const respondToTask = (data: {
    message: string;
    orderId: ITask["id"];
}) => {
    const formData = getFormData(data);

    return appAxios({
        url: `${responsesPath}`,
        method: "POST",
        data: formData,
    });
};

export const getCanRespond = (
    data: { orderId: ITask["id"] },
    token: string
) => {
    return appAxios.get(`${tasksPath}/CanRespond`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
        params: { orderId: data.orderId },
    });
};
