import { useState } from "react";
import {
    Typography,
    Paper,
    Divider,
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Grid,
} from "@mui/material";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { ProfileInfo, Page } from "../../components";
import "./accountPage.less";

export const AccountPage = () => {
    const [expanded, setExpanded] = useState<string | false>("panel1");

    const handleChange = (panel: string) => (_: any, isExpanded: boolean) => {
        setExpanded(isExpanded ? panel : false);
    };

    return (
        <Page className="account-page">
            <Grid container spacing={4}>
                <Grid item xs={12} md={5}>
                    <Paper className="account-card" elevation={8}>
                        <AccountCircleIcon className="account-img" />
                        <Divider />
                        <p className="info-row name">
                            Яблонский Кирилл Дмитриевич
                        </p>
                        <p className="info-row">blackshark564@gmail.com</p>
                        <p className="info-row">+375447010025</p>
                    </Paper>
                </Grid>
                <Grid item xs={12} md={12} lg={7}>
                    <Accordion
                        expanded={expanded === "panel1"}
                        onChange={handleChange("panel1")}
                    >
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel1bh-content"
                            id="panel1bh-header"
                        >
                            <Typography sx={{ width: "33%", flexShrink: 0 }}>
                                Ваши заказы
                            </Typography>
                            <Typography sx={{ color: "text.secondary" }}>
                                Заказы, которые вы разместили ранее
                            </Typography>
                        </AccordionSummary>
                        <Divider />
                        <AccordionDetails>
                            <Typography>
                                Nulla facilisi. Phasellus sollicitudin nulla et
                                quam mattis feugiat. Aliquam eget maximus est,
                                id dignissim quam.
                            </Typography>
                        </AccordionDetails>
                    </Accordion>
                    <Accordion
                        expanded={expanded === "panel2"}
                        onChange={handleChange("panel2")}
                    >
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel2bh-content"
                            id="panel2bh-header"
                        >
                            <Typography sx={{ width: "33%", flexShrink: 0 }}>
                                Запросы работ
                            </Typography>
                            <Typography sx={{ color: "text.secondary" }}>
                                Отклики на ваши заявки
                            </Typography>
                        </AccordionSummary>
                        <Divider />
                        <AccordionDetails>
                            <Typography>
                                Donec placerat, lectus sed mattis semper, neque
                                lectus feugiat lectus, varius pulvinar diam eros
                                in elit. Pellentesque convallis laoreet laoreet.
                            </Typography>
                        </AccordionDetails>
                    </Accordion>
                </Grid>
                <Grid item xs={12}>
                    <ProfileInfo
                        user={{
                            firstName: "Кирилл",
                            lastName: "Яблонский",
                            patronym: "Дмитриевич",
                            email: "blackshark564@gmail.com",
                            phoneNubmer: "+375447010025",
                            location: "Минск",
                            password: "1234",
                        }}
                    />
                </Grid>
            </Grid>
        </Page>
    );
};
