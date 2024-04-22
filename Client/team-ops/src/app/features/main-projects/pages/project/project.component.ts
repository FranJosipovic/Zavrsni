import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { UserStore } from '../../../../store/user.store';
import { User } from '../../../../core/user/interfaces';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrl: './project.component.scss',
})
export class ProjectComponent implements OnInit {
  private baseRoute: string;

  public userState: User;

  constructor(
    private userStore: UserStore,
    private route: ActivatedRoute,
    private router: Router,
    private renderer: Renderer2,
    private el: ElementRef
  ) {
    this.userState = userStore.getState().user!;
    this.baseRoute = router.url;
  }
  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      console.log(data);
      localStorage.setItem('projectId', data['ids'].projectId);
    });
    let route = this.buildRoute();
    this.router.navigateByUrl(route);
    // this.setDynamicMaxHeight()
  }

  setDynamicMaxHeight() {
    const windowHeight = window.innerHeight;
    const drawerContainer =
      this.el.nativeElement.querySelector('.content-container');
    const drawerContent = this.el.nativeElement.querySelector(
      '.mat-drawer-content-content-wrapper'
    );

    const containerHeight = drawerContainer.offsetHeight;
    const headerFooterHeight = containerHeight - drawerContent.offsetHeight;

    // Calculate dynamic max height
    let headerHeight = 64;
    let padding = 50;
    const dynamicMaxHeight =
      windowHeight - headerFooterHeight - headerHeight - padding;

    // Set dynamic max height to .mat-drawer-content-content-wrapper
    this.renderer.setStyle(
      drawerContent,
      'max-height',
      dynamicMaxHeight + 'px'
    );
  }

  public menu = [
    {
      title: 'Overview',
      isSelected: true,
      route: 'overview',
      children: [
        { title: 'Summary', isSelected: true, route: 'summary' },
        { title: 'Wiki', isSelected: false, route: 'wiki' },
      ],
    },
    {
      title: 'Boards',
      isSelected: false,
      route: 'boards',
      children: [
        { title: 'Work Items', isSelected: false, route: 'work-items' },
        { title: 'Sprints', isSelected: false, route: 'sprints' },
      ],
    },
    {
      title: 'Repos',
      isSelected: false,
      route: 'repos',
      children: [
        { title: 'Files', isSelected: false, route: 'files' },
        { title: 'Commits', isSelected: false, route: 'commits' },
      ],
    },
  ];

  onSelectParentItemMenu(parentTitle: string) {
    this.menu.forEach((menuItem) => {
      if (menuItem.title === parentTitle) {
        menuItem.isSelected = true;
        if (menuItem.children && menuItem.children.length > 0) {
          menuItem.children.forEach((child, index) => {
            if (index === 0) {
              child.isSelected = true;
            }
          });
        }
      } else {
        menuItem.isSelected = false;
        if (menuItem.children) {
          menuItem.children.forEach((child) => {
            child.isSelected = false;
          });
        }
      }
    });
    let route = this.buildRoute();
    this.router.navigateByUrl(route);
  }

  onSelectChildItemMenu(parentTitle: string, childTitle: string) {
    this.menu.forEach((parent) => {
      if (parent.title === parentTitle) {
        parent.children.forEach((child) => {
          if (child.title === childTitle) {
            child.isSelected = true;
          } else {
            child.isSelected = false;
          }
        });
      }
    });
    let route = this.buildRoute();
    this.router.navigateByUrl(route);
  }

  buildRoute() {
    let route = '';
    this.menu.forEach((item) => {
      if (item.isSelected) {
        route = `/${item.route}`;
        item.children.forEach((child) => {
          if (child.isSelected) {
            route = route + `/${child.route}`;
          }
        });
      }
    });
    return this.baseRoute + route;
  }
}
