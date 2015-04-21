#### [JsonPersister.apphb.com](http://jsonpersister.apphb.com) is a simple web site that store JSON objects for you. I did it for personal use when I am doing simple web/mobile prototypes, and have to store simple objects in the cloud.

## How to use:

**{app-id}** is an Id assigned by your application, it is simply a prefix to all of your resources
**{resource-id}** is the Id you want to use for the specific resource

#### Add or Update a resource using POST: 

```js
$.ajax({
	type: 'POST',
	url: 'http://jsonpersister.apphb.com/api/values/{app-id}',
	data: { firstName: 'renan', lastName: 'stigliani', email: 'renan@email.com' },
	dataType: 'json',
	success: function(id) {
		console.log('Resorce stored with id: ' + id)
	}
});
```

Alternatively one can use **PUT** and specify the resource id:

```js
$.ajax({
	type: 'POST',
	url: 'http://jsonpersister.apphb.com/api/values/{app-id}/{resource-id}',
	data: { firstName: 'renan', lastName: 'stigliani', email: 'renan@email.com' },
	dataType: 'json',
	success: function(id) {
		console.log('Resorce stored')
	}
});
```

#### To GET one specific resource:

```js
$.ajax({
	type: 'GET',
	url: 'http://jsonpersister.apphb.com/api/values/{app-id}/{resource-id}',
	dataType: 'json',
	success: function(data) {
		console.log(data)
	},
});
```

And to **GET all** of your resources use:

```js
$.ajax({
	type: 'GET',
	url: 'http://jsonpersister.apphb.com/api/values/{app-id}',
	dataType: 'json',
	success: function(data) {
		console.log(data)
	}
});
```

#### To DELETE a resource:

```js
$.ajax({
	type: 'DELETE',
	url: 'http://jsonpersister.apphb.com/api/values/{app-id}/{resource-id}',
	dataType: 'json',
	success: function() {
		console.log('Resorce deleted')
	}
});
```

To **DELETE all** of your resources:

```js
$.ajax({
	type: 'DELETE',
	url: 'http://jsonpersister.apphb.com/api/values/{app-id}',
	dataType: 'json',
	success: function() {
		console.log('All resources deleted')
	}
});
```
