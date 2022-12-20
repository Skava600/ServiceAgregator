import { Routes, Route, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { useCookies } from "react-cookie";
import { ErrorPage } from "./pages";
import { authorizedRoutes, routes } from "./routes/routes";
import { getUser } from "./state/selectors/userSelectors";
import { useAppDispatch, useAppSelector } from "./state/store";
import { getAccountInfo } from "./api";
import { loginSuccess } from "./state/slices/authSlice";

export const App = () => {
    const user = useAppSelector(getUser);
    const dispatch = useAppDispatch();
    const [cookies] = useCookies(["jwt"]);

    useEffect(() => {
        if (cookies.jwt) {
            getAccountInfo(cookies.jwt as string).then(({ data }) =>
                dispatch(
                    loginSuccess({ user: data, token: cookies.jwt as string })
                )
            );
        }
    }, [dispatch, cookies]);

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
