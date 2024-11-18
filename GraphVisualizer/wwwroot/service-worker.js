// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
self.addEventListener('fetch', event => { if (event.request.url.endsWith('.dll')) { event.respondWith(fetch(event.request)); } else { event.respondWith(caches.match(event.request).then(response => { return response || fetch(event.request); })); } });