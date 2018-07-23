import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router'
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ModalComponent } from './modal/modal.component';
import { MenuItems } from './menu-items/menu-items';
import { AccordionAnchorDirective, AccordionLinkDirective, AccordionDirective } from './accordion';
import { NgSelectModule } from '@ng-select/ng-select';
import { CustomSelect} from './select/custom-select';

@NgModule({
    declarations: [
      ModalComponent,
      AccordionAnchorDirective,
      AccordionLinkDirective,
      AccordionDirective,
      CustomSelect
    ],
    imports: [
        CommonModule,
        RouterModule,
        NgSelectModule,
        ReactiveFormsModule,
        FormsModule
    ],
    exports: [
      ModalComponent,
      AccordionAnchorDirective,
      AccordionLinkDirective,
      AccordionDirective,
      CustomSelect
    ],
    providers: [MenuItems]
})
export class CommonAppModule {

}
