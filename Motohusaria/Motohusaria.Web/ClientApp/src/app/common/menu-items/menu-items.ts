import { Injectable } from '@angular/core';

export interface BadgeItem {
  type: string;
  value: string;
}

export interface ChildrenItems {
  state: string;
  name: string;
  type?: string;
}

export interface Menu {
  state: string;
  name: string;
  type: string;
  icon: string;
  badge?: BadgeItem[];
  children?: ChildrenItems[];
}

const MENUITEMS = [
    {
        state: "user",
        name: "User",
        type: "link",
        icon: 'basic-sheet-txt'
    },
    {
        state: "role",
        name: "Role",
        type: "link",
        icon: 'basic-sheet-txt'
    } 
];

@Injectable()
export class MenuItems {
  getAll(): Menu[] {
    return MENUITEMS;
  }
}
