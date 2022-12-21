import { Paper, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import { Page } from "../../components";

export const PremiumPage = () => {
    return (
        <Page>
            <Paper>
                <Typography>
                    Спасибо, что приобрели нашу премиум подписку. Будьте
                    уверены, она оправдает свои ожидания и окупится сполна
                </Typography>
                <Link to="/"> Вернуться на главную</Link>
            </Paper>
        </Page>
    );
};
