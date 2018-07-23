import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  HostListener,
  NgZone,
  AfterViewInit
} from "@angular/core";
import { Title } from "@angular/platform-browser";
import { Router, ActivatedRoute, NavigationEnd } from "@angular/router";
import { NgbModal, ModalDismissReasons } from "@ng-bootstrap/ng-bootstrap";

import { MenuItems } from "../../common/menu-items/menu-items";
import { Subscription } from "rxjs";
import { filter } from "rxjs/operators";
import { TranslateService } from "@ngx-translate/core";
import { AuthService } from "../../authentication/auth.service";

const SMALL_WIDTH_BREAKPOINT = 991;

export interface Options {
  heading?: string;
  removeFooter?: boolean;
  mapHeader?: boolean;
}

@Component({
  selector: "app-layout",
  templateUrl: "./admin-layout.component.html",
  styleUrls: ["./admin-layout.component.scss"]
})
export class AdminLayoutComponent implements OnInit, OnDestroy, AfterViewInit {
  private _router: Subscription;
  private mediaMatcher: MediaQueryList = matchMedia(
      `(max-width: ${SMALL_WIDTH_BREAKPOINT}px)`
  );

  currentLang = "en";
  options: Options;
  theme = "light";
  showSettings = false;
  isDocked = false;
  isBoxed = false;
  isOpened = true;
  mode = "push";
  _mode = this.mode;
  _autoCollapseWidth = 991;
  width = window.innerWidth;

  @ViewChild("sidebar") sidebar;

  constructor(
      public menuItems: MenuItems,
      private router: Router,
      private route: ActivatedRoute,
      public translate: TranslateService,
      private modalService: NgbModal,
      private titleService: Title,
      private zone: NgZone,
      private authService: AuthService
  ) {
      const browserLang: string = translate.getBrowserLang();
      translate.use(browserLang.match(/en|fr/) ? browserLang : "en");
      this.mediaMatcher.addListener(mql =>
          zone.run(() => (this.mediaMatcher = mql))
      );
  }

  ngOnInit(): void {
      if (this.isOver()) {
          this._mode = "over";
          this.isOpened = false;
      }

      this._router = this.router.events
          .pipe(filter(event => event instanceof NavigationEnd))
          .subscribe((event: NavigationEnd) => {
              // Scroll to top on view load
              document.querySelector(".main-content").scrollTop = 0;
              this.runOnRouteChange();
          });
  }

  ngAfterViewInit(): void {
      setTimeout(_ => this.runOnRouteChange());
  }

  ngOnDestroy() {
      this._router.unsubscribe();
  }

  runOnRouteChange(): void {
      if (this.isOver() || this.router.url === "/maps/fullscreen") {
          this.isOpened = false;
      }

      this.route.children.forEach((route: ActivatedRoute) => {
          let activeRoute: ActivatedRoute = route;
          while (activeRoute.firstChild) {
              activeRoute = activeRoute.firstChild;
          }
          this.options = activeRoute.snapshot.data;
      });

      if (this.options) {
          if (this.options.hasOwnProperty("heading")) {
              this.setTitle(this.options.heading);
          }
      }
  }

  setTitle(newTitle: string) {
      this.titleService.setTitle(
          "Decima - Bootstrap 4 Angular Admin Template | " + newTitle
      );
  }

  toogleSidebar(): void {
      if (this._mode !== "dock") {
          this.isOpened = !this.isOpened;
      }
  }

  isOver(): boolean {
      return window.matchMedia(`(max-width: 991px)`).matches;
  }

  getUserName(): string | null {
      return this.authService.Name;
  }

  
  signOut(){
    this.authService.signout();
    this.router.navigateByUrl('/signin');
}
}
