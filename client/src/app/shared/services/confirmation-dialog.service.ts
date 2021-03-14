import {
  ApplicationRef,
  ComponentFactory,
  ComponentFactoryResolver,
  ComponentRef,
  EmbeddedViewRef,
  Injectable,
  Injector
} from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { take } from 'rxjs/operators';

import { ConfirmationDialogComponent } from '../components';
import { ConfirmationDialogData } from '../components/confirmation-dialog/confirmation-dialog.data';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationDialogService {
  private confirmationSubject = new Subject<boolean>();
  private confirmationDialogComponentRef: ComponentRef<ConfirmationDialogComponent>;

  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private applicationRef: ApplicationRef,
    private injector: Injector
  ) {}

  open(data: ConfirmationDialogData): void {
    const componentRef = this.createConfirmationDialogComponent(data);

    this.appendConfirmationDialogToView(componentRef);

    this.removeConfirmationDialogFromViewOnClose(componentRef);

    this.confirmationDialogComponentRef = componentRef;
  }

  afterClosed(): Observable<boolean> {
    return this.confirmationSubject.asObservable().pipe(take(1));
  }

  private createConfirmationDialogComponent(data: ConfirmationDialogData): ComponentRef<ConfirmationDialogComponent> {
    const componentFactory: ComponentFactory<ConfirmationDialogComponent> = this.componentFactoryResolver.resolveComponentFactory(
      ConfirmationDialogComponent
    );

    const componentRef = componentFactory.create(this.injector);

    componentRef.instance.data = {
      message: data.message,
      cancelButtonText: data.cancelButtonText || 'Cancel',
      confirmButtonText: data.confirmButtonText || 'OK'
    };

    return componentRef;
  }

  private appendConfirmationDialogToView(componentRef: ComponentRef<ConfirmationDialogComponent>) {
    this.applicationRef.attachView(componentRef.hostView);

    const domElement = (componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;

    document.body.appendChild(domElement);
  }

  private removeConfirmationDialogFromViewOnClose(componentRef: ComponentRef<ConfirmationDialogComponent>): void {
    const subscription = componentRef.instance.confirmationEvent.subscribe((confirmation) => {
      this.confirmationSubject.next(confirmation);

      this.applicationRef.detachView(this.confirmationDialogComponentRef.hostView);

      this.confirmationDialogComponentRef.destroy();

      subscription.unsubscribe();
    });
  }
}
