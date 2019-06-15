```
app.Map("/a", d => d.RunProxy(context => context
    .ForwardTo("http://10.0.75.1:5002/a")
    .Send()));
app.Map("/b", d => d.RunProxy(context => context
    .ForwardTo("http://10.0.75.1:5002/b/")
    .Send()));

location /a {
            proxy_pass   http://10.0.75.1:5002/a;
        }

location /b {
    proxy_pass   http://10.0.75.1:5002/b/;
}
```

| Request             | Nginx | ProxyKit |
|---------------------|-------|----------|
| http://localhost/a  | /a    | /a       |
| http://localhost/a/ | /a/   | /a/      |
| http://localhost/b  | /b/   | /b/      |
| http://localhost/b/ | /b//  | /b/      |