import { useEffect } from "react";
import { Link } from "react-router-dom";
import { Paper, Typography } from "@mui/material";
import { Page } from "../../components";
import { getToken } from "../../state/selectors/userSelectors";
import { useAppSelector } from "../../state/store";
import { postPayments } from "../../api";
import "./premiumPage.less";

export const PremiumPage = () => {
    const token = useAppSelector(getToken);

    useEffect(() => {
        if (!token) return;

        postPayments(token);
    }, [token]);

    return (
        <Page className="prem-page">
            <Paper className="prem-content">
                <Typography variant="h4">
                    Спасибо, что приобрели нашу премиум подписку. Будьте
                    уверены, она оправдает свои ожидания и окупится сполна!
                </Typography>
                <p>
                    <Link to="/">Вернуться на главную</Link>
                </p>
            </Paper>
        </Page>
    );
};
