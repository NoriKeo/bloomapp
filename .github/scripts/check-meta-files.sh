#!/usr/bin/env bash
# Unity lint: every file/folder in Assets/ needs a .meta file, and every .meta
# needs its asset. A missing or orphaned .meta breaks references (GUIDs) for
# everyone else who clones the repo.
set -euo pipefail

status=0

while IFS= read -r -d '' path; do
  if [[ ! -e "${path}.meta" ]]; then
    echo "::error file=${path}::Missing .meta file for ${path}"
    status=1
  fi
done < <(find Assets -mindepth 1 ! -name '*.meta' ! -name '.*' -print0)

while IFS= read -r -d '' meta; do
  asset="${meta%.meta}"
  if [[ ! -e "${asset}" ]]; then
    echo "::error file=${meta}::Orphaned .meta file (asset ${asset} does not exist)"
    status=1
  fi
done < <(find Assets -name '*.meta' -print0)

if [[ $status -eq 0 ]]; then
  echo "All .meta files OK."
fi
exit $status
