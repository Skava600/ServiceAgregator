import {
    ROOT_ROUTE,
    LOGIN_ROUTE,
    REGISTER_ROUTE,
    ACCOUNT_ROUTE,
    EDIT_PROFILE_ROUTE,
    PROFILES_ROUTE,
    PROFILE_ROUTE,
    CREATE_TASK_ROUTE,
    EDIT_TASK_ROUTE,
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
    EditTaskPage,
    CreateTaskPage,
    TasksPage,
    TaskPage,
} from "../pages";

export const authorizedRoutes = [
    {
        path: ACCOUNT_ROUTE,
        element: <AccountPage />,
    },
    {
        path: EDIT_TASK_ROUTE + "/:id",
        element: <EditTaskPage />,
    },
    {
        path: CREATE_TASK_ROUTE,
        element: <CreateTaskPage />,
    },
];

export const routes = [
    {
        path: ROOT_ROUTE,
        element: <TasksPage />,
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
