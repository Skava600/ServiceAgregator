import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Paper, Rating } from "@mui/material";
import FaceIcon from "@mui/icons-material/Face";
import { getProfile } from "../../api";
import { IProfile, IWorkCategory } from "../../api/interfaces";
import { Page } from "../../components";
import "./profilePage.less";

const reviewWordForms = ["отзыв", "отзыва", "отзывов"];
const orderWordForms = ["заказ", "заказа", "заказов"];

function getWordForm(n: number, wordForms: string[]) {
    n = Math.abs(n) % 100;
    var n1 = n % 10;

    if (n > 10 && n < 20) return wordForms[2];

    if (n1 > 1 && n1 < 5) return wordForms[1];

    if (n1 === 1) return wordForms[0];

    return wordForms[2];
}

const styleSummaryItem = (n: number, wordForms: string[]) => {
    return `${n || 0} ${getWordForm(n, wordForms)}`;
};

const INITIAL_STATE = {
    isLoading: true,
    profile: {} as IProfile,
    categories: [] as IWorkCategory[],
};

export const ProfilePage = () => {
    const { id } = useParams();
    const [isLoading, setIsLoading] = useState(INITIAL_STATE.isLoading);
    const [profile, setProfile] = useState(INITIAL_STATE.profile);
    const [categories, setCategories] = useState(INITIAL_STATE.categories);

    useEffect(() => {
        if (id) {
            getProfile({ id }).then(({ data }) => {
                setIsLoading(false);
                setProfile(data);
                console.log(data);
            });
        }
    }, [id]);

    const page = (
        <>
            <div className="first-row">
                <Paper className="profile-rating">
                    <div className="profile-summary">
                        <FaceIcon className="profile-avatar" />
                        <Rating size="large" value={profile.rating} readOnly />
                        <span className="item">{`${styleSummaryItem(
                            profile.reviewsCount,
                            reviewWordForms
                        )}`}</span>
                        <span className="item">{`${styleSummaryItem(
                            profile.orderCount,
                            orderWordForms
                        )} выполнено`}</span>
                    </div>
                </Paper>
                <Paper className="profile-header">
                    <div className="profile-summary-right">
                        <p>
                            <b>{profile.doerName}</b>
                        </p>
                        {profile.doerDescription}
                    </div>
                </Paper>
            </div>
            <Paper className="rest">
                {profile.sections?.map((sect) => sect).join()}
            </Paper>
        </>
    );

    return (
        <Page className="profile-page">
            {isLoading ? <h1>LOADING</h1> : page}
        </Page>
    );
};
