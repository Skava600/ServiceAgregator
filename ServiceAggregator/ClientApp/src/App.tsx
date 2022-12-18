import { Routes, Route } from "react-router-dom";
import { ErrorPage } from "./pages";
import { routes } from "./routes/routes";

export const App = () => {
    return (
        <Routes>
            <Route path={routes[0].path} element={routes[0].element} />
            {routes.map(({ path, element }) => {
                return <Route path={path} element={element} key={path} />;
            })}
            <Route path="*" element={<ErrorPage />} />
        </Routes>
    );
};
