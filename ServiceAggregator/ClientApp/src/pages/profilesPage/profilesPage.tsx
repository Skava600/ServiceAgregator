import { useCallback, useEffect, useState } from "react";
import { Card } from "@mui/material";
import {
    Page,
    ProfileCard,
    CategoryTree,
    ProgressSpinner,
} from "../../components/";
import { getProfiles, getWorkSections } from "../../api";
import { IWorkCategory, IWorkSection, IProfile } from "../../api/interfaces";
import "./profilesPage.less";

const INITIAL_STATE = {
    workSections: [] as IWorkCategory[],
    selectedSections: [] as IWorkSection[],
    profiles: [] as IProfile[],
    isProfilesDataLoading: true,
    isWSLoading: true,
};

export const ProfilesPage = () => {
    const [workSections, setWorkSections] = useState(
        INITIAL_STATE.workSections
    );
    const [selectedSections, setSelectedSections] = useState(
        INITIAL_STATE.selectedSections
    );
    const [profiles, setProfiles] = useState(INITIAL_STATE.profiles);
    const [isProfilesDataLoading, setIsProfilesDataLoading] = useState(
        INITIAL_STATE.isProfilesDataLoading
    );
    const [isWSLoading, setIsWSLoading] = useState(INITIAL_STATE.isWSLoading);

    const fetchProfiles = useCallback(() => {
        setIsProfilesDataLoading(true);
        getProfiles(selectedSections.map(({ slug }) => slug))
            .then(({ data }) => setProfiles(data))
            .finally(() => setIsProfilesDataLoading(false));
    }, [selectedSections]);

    useEffect(() => {
        getWorkSections()
            .then(({ data }) => setWorkSections(data))
            .finally(() => setIsWSLoading(false));
        fetchProfiles();
    }, [fetchProfiles]);

    const handleAddSection = (section: IWorkSection, shouldSelect: boolean) => {
        const index = selectedSections.findIndex(
            (s) => s.slug === section.slug
        );
        const exists = index !== -1;

        if (shouldSelect && !exists) {
            setSelectedSections((v) => [...v, section]);
            return;
        }

        if (!shouldSelect && exists) {
            const sections = [...selectedSections];
            sections.splice(index, 1);
            setSelectedSections(sections);
        }
    };

    return (
        <Page className="profiles-page" navbarTabIndex={1}>
            {isWSLoading ? (
                <ProgressSpinner />
            ) : (
                <div className="divider">
                    <CategoryTree
                        workSections={workSections}
                        selectedSections={selectedSections}
                        handleAddSection={handleAddSection}
                    />
                    <div className="right-side">
                        {isProfilesDataLoading ? (
                            <ProgressSpinner />
                        ) : (
                            <>
                                {profiles.map((profile) => (
                                    <ProfileCard profile={profile} />
                                ))}
                                {!profiles.length && (
                                    <Card className="no-data">
                                        <b>
                                            По вашему запросу сейчас нет
                                            исполнителей
                                        </b>
                                    </Card>
                                )}
                            </>
                        )}
                    </div>
                </div>
            )}
        </Page>
    );
};
