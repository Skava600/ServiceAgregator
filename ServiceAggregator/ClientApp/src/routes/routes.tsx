import { ROOT_ROUTE, TASK_ROUTE, LOGIN_ROUTE, REGISTER_ROUTE } from "./pathes";
import { LoginPage } from "../pages";

export const routes = [
    {
        path: ROOT_ROUTE,
        element: <div>s</div>,
    },
    {
        path: LOGIN_ROUTE,
        element: <LoginPage />,
    },
    {
        path: TASK_ROUTE + "/:id",
        element: <div>s2</div>,
    },
];
