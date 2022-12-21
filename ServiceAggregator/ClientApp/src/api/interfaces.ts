export interface IWorkSection {
    name: string;
    slug: string;
    categoryName?: string;
}

export interface IUser {
    firstname: string;
    lastname: string;
    patronym: string;
    phoneNumber: string;
    location: string;
    login: string;
    doerId: string | null;
    isAdmin: boolean;
    hasPremium: boolean;
}

export interface IWorkCategory {
    name: string;
    sections: IWorkSection[];
}

export interface IProfile {
    id: string;
    doerDescription: string;
    doerName: string;
    orderCount: number;
    reviewsCount: number;
    rating: number;
    reviews: any[];
    sections: IWorkSection[];
}

type TTaskStatus = "Open" | "Done" | "Canceled" | "InProgress";

export interface ITask {
    id: string;
    header: string;
    text: string;
    location: string;
    status: TTaskStatus;
    expireDate: string;
    price: number | null;
    customer: {
        id: string;
        rating: number;
        account: {
            firstname: string;
            lastname: string;
            patronym: string;
            phoneNumber: string;
            location?: string;
        };
        reviews?: any;
    };
    section: IWorkSection;
    orderResult: string;
    responseCount: number;
}

export interface ITaskResponse {
    message: string;
    doer: IProfile;
    isChosen: boolean;
}
