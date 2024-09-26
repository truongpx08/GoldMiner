mergeInto(LibraryManager.library, {
  Reopen: function () {
    window.dispatchReactUnityEvent("Reopen");
  },
});