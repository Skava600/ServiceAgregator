import { Link } from "react-router-dom";
import { Tab, Typography } from "@mui/material";
import { Page } from "../../components/";
import "./mainPage.less";

export const MainPage = () => {
    return (
        <Page className="main-page" navbarTabIndex={999}>
            <div className="content">
                <Typography variant="h1" color="white">
                    Добро пожаловать!
                </Typography>
            </div>
        </Page>
    );
};
