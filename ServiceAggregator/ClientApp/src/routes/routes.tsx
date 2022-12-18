import {
    ROOT_ROUTE,
    TASK_ROUTE,
    LOGIN_ROUTE,
    REGISTER_ROUTE,
    ACCOUNT_ROUTE,
    PROFILES_ROUTE,
    TASKS_ROUTE,
} from "./pathes";
import {
    MainPage,
    LoginPage,
    RegisterPage,
    AccountPage,
    ProfilesPage,
    TasksPage,
} from "../pages";

export const routes = [
    {
        path: ROOT_ROUTE,
        element: <MainPage />,
    },
    {
        path: REGISTER_ROUTE,
        element: <RegisterPage />,
    },
    {
        path: LOGIN_ROUTE,
        element: <LoginPage />,
    },
    {
        path: TASK_ROUTE + "/:id",
        element: <div>s2</div>,
    },
    {
        path: PROFILES_ROUTE,
        element: <ProfilesPage />,
    },
    {
        path: TASKS_ROUTE,
        element: <TasksPage />,
    },
    {
        path: ACCOUNT_ROUTE,
        element: <AccountPage />,
    },
];
