mergeInto(LibraryManager.library, {

  
  SendScore: function (score) {
  window.ReactNativeWebView.postMessage(Pointer_stringify(score));

}



  

});