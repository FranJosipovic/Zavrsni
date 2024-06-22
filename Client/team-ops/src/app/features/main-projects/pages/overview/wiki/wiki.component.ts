import { Component, OnInit } from '@angular/core';
import {
  ProjectWikiData,
  ProjectWikiNode,
  WikiService,
} from './services/wiki.service';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { UserStore } from '../../../../../store/user.store';
import { CreateOrUpdateWikiData } from './create-update-wiki/create-update-wiki.component';

@Component({
  selector: 'app-wiki',
  templateUrl: './wiki.component.html',
  styleUrl: './wiki.component.scss',
})
export class WikiComponent implements OnInit {

  private projectId: string;
  constructor(private wikiService: WikiService, private userStore: UserStore) {
    this.projectId = localStorage.getItem('projectId')!;
  }

  ngOnInit(): void {
    this.loadWikis();
  }
  //tree wikis data
  treeControl = new NestedTreeControl<ProjectWikiNode>((node) => node.children);
  dataSource = new MatTreeNestedDataSource<ProjectWikiNode>();

  hasChild = (_: number, node: ProjectWikiNode) =>
    !!node.children && node.children.length > 0;

  onNodeItemClick(id: string) {
    this.selectedNodeId = id;
    this.isEditingOrCreating = false;
    this.loadWikiData(id);
  }

  loadWikis() {
    this.dataSource.data = []
    this.selectedProjectWiki = null
    this.selectedNodeId = undefined
    this.wikiService.getWikis(this.projectId).subscribe((data) => {
      console.log(data);
      if (data.isSuccess) {
        if (data.data!.count > 0) {
          this.selectedNodeId = data.data?.items[0]?.id;
          this.dataSource.data = data.data?.items!;
          if (this.selectedNodeId && this.selectedNodeId.length > 0) {
            this.loadWikiData(this.selectedNodeId);
          }
        }
      }
    });
  }

  loadWikiData(id: string) {
    this.wikiService.getWikiData(id).subscribe((data) => {
      console.log(data);
      this.selectedProjectWiki = data.data;
    });
  }

  //create or update wiki
  public createOrUpdateWikiData: CreateOrUpdateWikiData | null = null;
  public selectedNodeId: string | undefined = '';
  public selectedProjectWiki: ProjectWikiData | null | undefined;
  public isEditingOrCreating: boolean = false;
  public shouldCreate!: boolean;
  
  editOrCreate(shouldCreate: boolean) {
    this.createOrUpdateWikiData = {
      wikiId: this.selectedProjectWiki?.id ?? null,
      title: this.selectedProjectWiki?.title ?? '',
      content: this.selectedProjectWiki?.content ?? '',
      parentId: this.selectedProjectWiki?.parentId ?? null,
      createdById: this.userStore.getState().user?.id!,
      projectId: this.projectId,
    };
    this.shouldCreate = shouldCreate
    this.isEditingOrCreating = true;
  }

  createNewWiki(){
    this.createOrUpdateWikiData = {
      wikiId: null,
      title: '',
      content: '',
      parentId: null,
      createdById: this.userStore.getState().user?.id!,
      projectId: this.projectId,
    };
    this.shouldCreate = true
    this.isEditingOrCreating = true;
  }

  createNewWikiChild(parentId:string){
    this.createOrUpdateWikiData = {
      wikiId: null,
      title: '',
      content: '',
      parentId: parentId,
      createdById: this.userStore.getState().user?.id!,
      projectId: this.projectId,
    };
    this.shouldCreate = true
    this.isEditingOrCreating = true;
  }

  editWiki(){
    this.createOrUpdateWikiData = {
      wikiId: this.selectedProjectWiki?.id ?? null,
      title: this.selectedProjectWiki?.title ?? '',
      content: this.selectedProjectWiki?.content ?? '',
      parentId: this.selectedProjectWiki?.parentId ?? null,
      createdById: this.userStore.getState().user?.id!,
      projectId: this.projectId,
    };
    this.shouldCreate = false
    this.isEditingOrCreating = true;
  }

  finishCreateOrEdit() {
    this.createOrUpdateWikiData = null;
    this.isEditingOrCreating = false;
    this.loadWikis();
  }

  deleteWiki(){
    this.selectedProjectWiki?.id && 
    this.wikiService.deleteWiki(this.selectedProjectWiki?.id).subscribe((res)=>{
      console.log(res)
      if(res.isSuccess){
        this.loadWikis()
      }
    })
  }
}
