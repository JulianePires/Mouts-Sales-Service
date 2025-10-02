#!/bin/bash

# Script to clean build artifacts and resolve conflicts
# This should be run whenever you encounter build artifact conflicts

echo "🧹 Cleaning all build artifacts..."

# Clean .NET solution
dotnet clean Ambev.DeveloperEvaluation.sln

# Remove all bin and obj directories
echo "📁 Removing bin and obj directories..."
find . -name "bin" -type d -exec rm -rf {} + 2>/dev/null || true
find . -name "obj" -type d -exec rm -rf {} + 2>/dev/null || true

# Remove any build artifacts that might be staged or in conflict
echo "🔧 Resolving any Git conflicts with build artifacts..."
git reset HEAD -- "**/bin/" "**/obj/" "**/*.dll" "**/*.pdb" "**/*.exe" "**/*.cache" "**/Debug/" "**/Release/" 2>/dev/null || true
git clean -fd "**/bin/" "**/obj/" 2>/dev/null || true

echo "✅ Build artifacts cleaned successfully!"
echo "💡 You can now continue with your merge/rebase operation."

# Verify clean state
echo "📊 Git status:"
git status --porcelain | grep -v "^??" || echo "Working tree is clean"