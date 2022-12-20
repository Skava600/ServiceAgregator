import { Routes, Route, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { ErrorPage } from "./pages";
import { authorizedRoutes, routes } from "./routes/routes";
import { getUser } from "./state/selectors/userSelectors";
import { useAppSelector } from "./state/store";

export const App = () => {
    const user = useAppSelector(getUser);
    const navigate = useNavigate();

    useEffect(() => {
        if (!user) {
            // navigate("/login");
        }
    }, [navigate, user]);

    return (
        <Routes>
            <Route path={routes[0].path} element={routes[0].element} />
            {routes.map(({ path, element }) => {
                return <Route path={path} element={element} key={path} />;
            })}
            {user &&
                authorizedRoutes.map(({ path, element }) => {
                    return <Route path={path} element={element} key={path} />;
                })}
            <Route path="*" element={<ErrorPage />} />
        </Routes>
    );
};
