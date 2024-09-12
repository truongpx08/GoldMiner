mergeInto(LibraryManager.library, {
  OnClickLinkButton: function () {
    window.dispatchReactUnityEvent("OnClickLinkButton");
  },
});
