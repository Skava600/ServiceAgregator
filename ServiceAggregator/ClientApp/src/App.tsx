import { useEffect } from "react";
import { Routes, Route } from "react-router-dom";
import axios from "axios";
import { ErrorPage } from "./pages";
import { routes } from "./routes/routes";

export const App = () => {
    useEffect(() => {
        axios
            .get(`api/WorkSections/GetListOfSections`)
            .then((res) => console.log(res));
    }, []);
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
