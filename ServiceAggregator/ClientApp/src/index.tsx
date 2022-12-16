import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import { CssVarsProvider } from "@mui/joy/styles";
import { App } from "./App";
import "./index.less";

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");

const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
);
root.render(
    <CssVarsProvider>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </CssVarsProvider>
);
