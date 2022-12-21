import {
    ROOT_ROUTE,
    LOGIN_ROUTE,
    REGISTER_ROUTE,
    ACCOUNT_ROUTE,
    CREATE_PROFILE_ROUTE,
    EDIT_PROFILE_ROUTE,
    PROFILES_ROUTE,
    PROFILE_ROUTE,
    CREATE_TASK_ROUTE,
    EDIT_TASK_ROUTE,
    TASKS_ROUTE,
    TASK_ROUTE,
    PREMIUM_ROUTE,
} from "./pathes";
import {
    LoginPage,
    RegisterPage,
    AccountPage,
    ProfilesPage,
    CreateProfilePage,
    ProfilePage,
    EditTaskPage,
    CreateTaskPage,
    TasksPage,
    TaskPage,
    PremiumPage,
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
    {
        path: CREATE_PROFILE_ROUTE,
        element: <CreateProfilePage />,
    },
    {
        path: EDIT_PROFILE_ROUTE + "/:id",
        element: <CreateProfilePage />,
    },
    {
        path: PREMIUM_ROUTE,
        element: <PremiumPage />,
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
