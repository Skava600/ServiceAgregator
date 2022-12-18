import { useEffect } from "react";
import { Routes, Route } from "react-router-dom";
import axios from "axios";
import { ErrorPage } from "./pages";
import { routes } from "./routes/routes";
import appAxios from "./api/axios";

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
