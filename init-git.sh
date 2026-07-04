#!/bin/bash
# Initialize git repository
git init

# Create root .gitignore
cat > .gitignore << EOL
node_modules/
dist/
dist-ssr/
*.local
.env
.env.*
.DS_Store
.idea/
.vscode/
*.suo
*.user
*.userosscache
*.sln.docstates
bin/
obj/
*.log
EOL

# Add and commit database
git add database/
git commit -m "feat(database): initialize SQL Server creation and seed scripts"

# Add and commit backend
git add backend/
git commit -m "feat(backend): implement Clean Architecture ASP.NET Core Web API with JWT and Dapper"

# Add and commit frontend
git add frontend/
git commit -m "feat(frontend): implement Vue 3 + Tailwind CSS + Pinia Notes application"

# Add and commit docs
git add docs/ README.md .gitignore
git commit -m "docs: add README and Postman collection"

echo "Git repository initialized and committed using Conventional Commits."
