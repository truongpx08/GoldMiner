mergeInto(LibraryManager.library, {
  GameStart: function () {
    window.dispatchReactUnityEvent("GameStart");
  },
});
