export interface Review {
    id?: string;
    rate: number;
    description: string;
    authorName: string;
    authorProfilePicFilename: string;
    targetId: number;
    targetProfilePicFilename: string;
}
