import { Notification } from './notifications/notification';

export interface ServiceResponse<T = null> {
    success: boolean;
    result: T | null;
    notifications: Notification[] | null;
}
