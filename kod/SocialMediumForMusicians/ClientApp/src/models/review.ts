export interface Review {
    id?: string;
    rate: number;
    description: string;
    authorName?: string;
    authorId: string;
    authorProfilePicFilename?: string;
    targetId: string;
    targetProfilePicFilename?: string;
    sentAt: Date;
}
