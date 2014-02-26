function hideDialogWhenUserClicksOutside(dialogId) {
    jQuery('html')
                      .bind(
                       'click',
                       function (e) {
                           if (
                            jQuery(dialogId).dialog('isOpen')
                            && !jQuery(e.target).is('.ui-dialog, a')
                            && !jQuery(e.target).closest('.ui-dialog').length
                           ) {
                               jQuery(dialogId).dialog('close');
                           }
                       }
                      );
}