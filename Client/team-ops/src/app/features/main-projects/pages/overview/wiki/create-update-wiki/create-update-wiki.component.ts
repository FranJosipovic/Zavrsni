import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { WikiService } from '../services/wiki.service';
import { ChangeEvent } from '@ckeditor/ckeditor5-angular';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ToastrService } from 'ngx-toastr';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-update-wiki',
  templateUrl: './create-update-wiki.component.html',
  styleUrl: './create-update-wiki.component.scss',
})
export class CreateUpdateWikiComponent implements OnInit {
  @Input() data!: CreateOrUpdateWikiData;
  @Input() shouldCreate!: boolean;
  @Output() finishEditOrCreate = new EventEmitter();

  constructor(
    private wikiService: WikiService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.title.setValue(this.data.title);
  }

  public title = new FormControl('', Validators.required);

  //editor
  public Editor = ClassicEditor;
  private editedContent: string = '';

  public onChange({ editor }: ChangeEvent) {
    this.editedContent = editor.getData();
  }

  //mange save/close
  close() {
    this.finishAction();
  }

  onFinish() {
    if (this.shouldCreate) {
      this.create();
    } else {
      this.update();
    }
  }

  finishAction() {
    this.finishEditOrCreate.emit();
  }

  //update or create calls
  private update() {
    console.log(this.title.value);
    this.wikiService
      .updateWiki(this.data.wikiId!, this.editedContent, this.title.value!)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.toastr.show(
            data.message,
            'Wiki',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-success'
          );
        } else {
          this.toastr.show(
            data.message,
            'Wiki',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-error'
          );
        }
        this.editedContent = '';
        this.finishAction();
      });
  }

  private create() {
    this.wikiService
      .createWiki(
        this.editedContent,
        this.title.value!,
        this.data.projectId,
        this.data.createdById,
        this.data.parentId
      )
      .subscribe((data) => {
        if (data.isSuccess) {
          this.toastr.show(
            data.message,
            'Wiki',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-success'
          );
        } else {
          this.toastr.show(
            data.message,
            'Wiki',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-error'
          );
        }
        this.editedContent = '';
        this.finishAction();
      });
  }
}

export interface CreateOrUpdateWikiData {
  wikiId: string | null;
  title: string;
  content: string;
  parentId: string | null;
  createdById: string;
  projectId: string;
}
