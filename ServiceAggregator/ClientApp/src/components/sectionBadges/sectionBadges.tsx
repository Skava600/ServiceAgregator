import { Chip } from "@mui/material";
import { IWorkSection } from "../../api/interfaces";
import "./sectionBadges.less";

type TProps = {
    sectionsList: IWorkSection[];
    onRemoveSection: (sectionSlug: IWorkSection) => void;
};

const SectionBadge = ({
    section,
    onRemove,
}: {
    section: IWorkSection;
    onRemove: () => void;
}) => {
    return (
        <Chip
            className="section-badge"
            label={section.name}
            variant="outlined"
            onDelete={onRemove}
        />
    );
};

export const SectionBadges = ({ sectionsList, onRemoveSection }: TProps) => {
    return (
        <div className="badges-wrapper">
            <div className="badges">
                {sectionsList.map((sect) => (
                    <SectionBadge
                        section={sect}
                        onRemove={() => onRemoveSection(sect)}
                    />
                ))}
            </div>
        </div>
    );
};
