import {
    ROOT_ROUTE,
    LOGIN_ROUTE,
    REGISTER_ROUTE,
    ACCOUNT_ROUTE,
    PROFILES_ROUTE,
    PROFILE_ROUTE,
    TASKS_ROUTE,
    TASK_ROUTE,
} from "./pathes";
import {
    MainPage,
    LoginPage,
    RegisterPage,
    AccountPage,
    ProfilesPage,
    ProfilePage,
    TasksPage,
    TaskPage,
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
        path: ACCOUNT_ROUTE,
        element: <AccountPage />,
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
        path: PROFILE_ROUTE + "/:id",
        element: <ProfilePage />,
    },
    {
        path: TASK_ROUTE + "/:id",
        element: <TaskPage />,
    },
];
