import { useEffect, useMemo, useState } from "react";
import { useParams } from "react-router-dom";
import { Divider, Paper, Rating } from "@mui/material";
import FaceIcon from "@mui/icons-material/Face";
import { getProfile } from "../../api";
import { IProfile, IWorkCategory, IWorkSection } from "../../api/interfaces";
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
};

export const ProfilePage = () => {
    const { id } = useParams();
    const [isLoading, setIsLoading] = useState(INITIAL_STATE.isLoading);
    const [profile, setProfile] = useState(INITIAL_STATE.profile);

    const _sections = useMemo(() => {
        return profile?.sections?.reduce((t, c) => {
            if (!c.categoryName) return t;

            if (!Array.isArray(t[c.categoryName])) {
                t[c.categoryName] = [];
            }

            t[c.categoryName] = [...t[c.categoryName], c];

            return t;
        }, {} as { [key: string]: IWorkSection[] });
    }, [profile?.sections]);

    useEffect(() => {
        if (id) {
            getProfile({ id }).then(({ data }) => {
                setIsLoading(false);
                setProfile(data);
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
                <b className="rest-header">Оказываемые услуги:</b>
                {_sections &&
                    Object.keys(_sections).reduce((t, c, i) => {
                        t.push(
                            <div className="category" key={c}>
                                {c}
                                <ul className="category-list">
                                    {Object.values(_sections)[i].map((sect) => (
                                        <li>
                                            <span
                                                className="section"
                                                key={sect.slug}
                                            >
                                                {sect.name}
                                            </span>
                                        </li>
                                    ))}
                                </ul>
                                <Divider />
                            </div>
                        );
                        return t;
                    }, [] as any)}
            </Paper>

            <Paper className="rest">
                <b className="rest-header">Отзывы:</b>
                {profile?.reviews?.length ? (
                    profile?.reviews?.map((r) => (
                        <div className="review">
                            <div className="review-header">
                                <span>{r.customerAuthor}</span>
                                <Rating value={r.grade} readOnly size="small" />
                            </div>
                            <p>{r.text}</p>
                            <Divider />
                        </div>
                    ))
                ) : (
                    <p>У этого исполнителя пока нет отзывов</p>
                )}
            </Paper>
        </>
    );

    return (
        <Page className="profile-page">
            {isLoading ? <h1>LOADING</h1> : page}
        </Page>
    );
};
